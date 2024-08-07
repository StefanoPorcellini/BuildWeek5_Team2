using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        var vendita = await _venditaService.GetByIdAsync(id);

        if (vendita == null)
        {
            return NotFound();
        }

        var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 25, 25);
        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        writer.CloseStream = false;
        document.Open();

        // Aggiungere il logo
        var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", "logo_vetcare.png");
        if (System.IO.File.Exists(logoPath))
        {
            Image logo = Image.GetInstance(logoPath);
            logo.ScaleAbsolute(100, 50);
            logo.Alignment = Element.ALIGN_CENTER;
            document.Add(logo);
        }

        document.Add(new Paragraph(" ")); // Aggiungere uno spazio vuoto

        // Titolo del PDF
        var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
        var title = new Paragraph("Dettagli della Vendita", titleFont)
        {
            Alignment = Element.ALIGN_CENTER
        };
        document.Add(title);

        document.Add(new Paragraph(" ")); // Aggiungere uno spazio vuoto

        var table = new PdfPTable(2)
        {
            WidthPercentage = 80,
            HorizontalAlignment = Element.ALIGN_CENTER
        };
        table.SetWidths(new float[] { 1, 2 });

        // Aggiungere celle alla tabella
        AddCellToHeader(table, "Codice Fiscale Cliente:");
        AddCellToData(table, vendita.CodiceFiscaleCliente);

        AddCellToHeader(table, "Data Vendita:");
        AddCellToData(table, vendita.DataVendita.ToString("dd/MM/yyyy HH:mm"));

        AddCellToHeader(table, "Prodotto:");
        AddCellToData(table, vendita.Prodotto.Nome);

        AddCellToHeader(table, "Prezzo Unitario:");
        AddCellToData(table, vendita.Prodotto.Prezzo.ToString("C"));

        AddCellToHeader(table, "Quantità:");
        AddCellToData(table, vendita.Quantita.ToString());

        var totale = vendita.Quantita * vendita.Prodotto.Prezzo;
        AddCellToHeader(table, "Totale:");
        AddCellToData(table, totale.ToString("C"));

        document.Add(table);
        document.Close();

        memoryStream.Position = 0;
        return File(memoryStream, "application/pdf", $"Vendita_{id}.pdf");
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
