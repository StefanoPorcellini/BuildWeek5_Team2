using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Farmacista,Admin")]
public class VenditeController : Controller
{
    private readonly IVenditaService _venditaService;
    private readonly VeterinaryClinicContext _context;
    private readonly ILogger<VenditeController> _logger;


    public VenditeController(IVenditaService venditaService, VeterinaryClinicContext context, ILogger<VenditeController> logger)
    {
        _venditaService = venditaService;
        _context = context;
        _logger = logger;
    }

    // GET: Vendite
    public async Task<IActionResult> Index()
    {
        var vendite = await _venditaService.GetAllAsync();
        return View(vendite);
    }

    // GET: Vendite/Create
    public IActionResult Create()
    {
        ViewData["ProdottoId"] = new SelectList(_context.Prodotti, "Id", "Nome");
        return View();
    }

    // POST: Vendite/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CodiceFiscaleCliente,DataVendita,ProdottoId,NumeroRicetta,Quantita")] Vendita vendita)
    {
        _logger.LogInformation("Avvio del processo di creazione per una nuova vendita.");

        if (ModelState.IsValid)
        {
            try
            {
                _logger.LogInformation("ModelState è valido. Creazione della vendita per il prodotto ID: {ProdottoId}, Cliente: {CodiceFiscaleCliente}", vendita.ProdottoId, vendita.CodiceFiscaleCliente);

                await _venditaService.CreateAsync(vendita);

                _logger.LogInformation("Vendita creata con successo con ID: {VenditaId}", vendita.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione della vendita per il prodotto ID: {ProdottoId}, Cliente: {CodiceFiscaleCliente}", vendita.ProdottoId, vendita.CodiceFiscaleCliente);
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la creazione della vendita. Riprova più tardi.");
            }
        }
        else
        {
            _logger.LogWarning("ModelState non valido per la vendita. Dettagli degli errori:");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogWarning("Errore nel campo '{Field}': {ErrorMessage}", state.Key, error.ErrorMessage);
                }
            }
        }

        _logger.LogInformation("Ripopolazione del SelectList per il prodotto dopo errore nella creazione della vendita.");
        ViewData["ProdottoId"] = new SelectList(_context.Prodotti, "Id", "Nome", vendita.ProdottoId);

        _logger.LogInformation("Ritorno della vista di creazione con modello non valido.");
        return View(vendita);
    }

    // GET: Vendite/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vendita = await _venditaService.GetByIdAsync(id.Value);
        if (vendita == null)
        {
            return NotFound();
        }

        return View(vendita);
    }


    // GET: Vendite/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vendita = await _venditaService.GetByIdAsync(id.Value);
        if (vendita == null)
        {
            return NotFound();
        }
        ViewData["ProdottoId"] = new SelectList(_context.Prodotti, "Id", "Nome", vendita.ProdottoId);
        return View(vendita);
    }

    // POST: Vendite/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CodiceFiscaleCliente,DataVendita,ProdottoId,NumeroRicetta,Quantita")] Vendita vendita)
    {
        if (id != vendita.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _venditaService.UpdateAsync(vendita);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VenditaExists(vendita.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProdottoId"] = new SelectList(_context.Prodotti, "Id", "Nome", vendita.ProdottoId);
        return View(vendita);
    }


    private async Task<bool> VenditaExists(int id)
    {
        var vendita = await _venditaService.GetByIdAsync(id);
        return vendita != null;
    }

    public async Task<IActionResult> GeneratePDFPerVendita(int id)
    {
        // Recupera i dati della vendita dall'ID fornito
        var vendita = await _venditaService.GetByIdAsync(id);

        // Verifica se la vendita esiste
        if (vendita == null)
        {
            // Restituisce un errore 404 se la vendita non è trovata
            return NotFound();
        }

        // Costruisce il percorso del file del template PDF
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdf", "template_ricevuta_modulo.pdf");

        // Controlla se il file del template esiste
        if (!System.IO.File.Exists(templatePath))
        {
            // Logga un errore e restituisce un errore 500 se il template non esiste
            _logger.LogError("Il file del template PDF non esiste: {TemplatePath}", templatePath);
            return StatusCode(500, "Template PDF non trovato.");
        }

        try
        {
            // Crea un MemoryStream per memorizzare il PDF generato
            using (var memoryStream = new MemoryStream())
            {
                // Carica il PDF template e crea un PdfStamper per modificarlo
                using (var pdfReader = new PdfReader(templatePath))
                {
                    using (var pdfStamper = new PdfStamper(pdfReader, memoryStream))
                    {
                        // Ottieni i campi del modulo dal PDF
                        AcroFields formFields = pdfStamper.AcroFields;

                        // Compila i campi del modulo con i dati della vendita
                        formFields.SetField("CF", vendita.CodiceFiscaleCliente);
                        formFields.SetField("ID", vendita.Id.ToString());

                        // Ottieni la posizione e la larghezza del campo data per l'allineamento
                        string dataValue = vendita.DataVendita.ToString("dd/MM/yyyy HH:mm");
                        var dataFieldPosition = formFields.GetFieldPositions("data")[0];
                        float fieldWidth = dataFieldPosition.position.Width;

                        // Calcola il numero di spazi necessari per allineare il testo a destra
                        int averageCharWidth = 5; // Larghezza media del carattere, può essere regolata
                        int spacesNeeded = (int)((fieldWidth - (dataValue.Length * averageCharWidth)) / averageCharWidth);
                        string rightAlignedData = new string(' ', spacesNeeded) + dataValue;

                        // Imposta i valori dei campi nel modulo PDF
                        formFields.SetField("data", rightAlignedData);
                        formFields.SetField("prodotto", vendita.Prodotto.Nome);
                        formFields.SetField("quantita", vendita.Quantita.ToString());
                        formFields.SetField("Prezzo", vendita.Prodotto.Prezzo.ToString("C"));

                        // Calcola il totale e imposta il campo totale nel modulo PDF
                        var totale = vendita.Quantita * vendita.Prodotto.Prezzo;
                        formFields.SetField("totale", totale.ToString("C"));

                        // Conclude e chiude il PdfStamper e il PdfReader
                        pdfStamper.FormFlattening = true;
                    }
                }

                // Copia il contenuto del MemoryStream in un array di byte per restituirlo come file
                var pdfContent = memoryStream.ToArray();

                // Restituisce il PDF come file scaricabile
                var fileName = $"Vendita_{DateTime.Now:yyyyMMdd}_{id}.pdf";
                return File(pdfContent, "application/pdf", fileName);
            }
        }
        catch (Exception ex)
        {
            // Logga l'errore se si verifica un problema durante la generazione del PDF
            _logger.LogError(ex, "Errore durante la generazione del PDF per la vendita con ID: {VenditaId}", id);
            return StatusCode(500, "Errore durante la generazione del PDF.");
        }
    }


    private void AddCellToHeader(PdfPTable table, string text)
    {
        var font = FontFactory.GetFont("Arial", 12, Font.BOLD);
        var cell = new PdfPCell(new Phrase(text, font))
        {
            HorizontalAlignment = Element.ALIGN_RIGHT,
            Border = PdfPCell.NO_BORDER
        };
        table.AddCell(cell);
    }

    private void AddCellToData(PdfPTable table, string text)
    {
        var font = FontFactory.GetFont("Arial", 12, Font.NORMAL);
        var cell = new PdfPCell(new Phrase(text, font))
        {
            HorizontalAlignment = Element.ALIGN_LEFT,
            Border = PdfPCell.NO_BORDER
        };
        table.AddCell(cell);
    }



}
