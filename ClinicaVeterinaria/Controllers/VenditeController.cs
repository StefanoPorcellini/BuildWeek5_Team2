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

    public VenditeController(IVenditaService venditaService, VeterinaryClinicContext context)
    {
        _venditaService = venditaService;
        _context = context;
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
        if (ModelState.IsValid)
        {
            await _venditaService.CreateAsync(vendita);
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProdottoId"] = new SelectList(_context.Prodotti, "Id", "Nome", vendita.ProdottoId);
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

    // GET: Vendite/Delete/5
    public async Task<IActionResult> Delete(int? id)
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

    // POST: Vendite/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _venditaService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
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

        using (var memoryStream = new MemoryStream())
        {
            var document = new Document();
            PdfWriter.GetInstance(document, memoryStream).CloseStream = false;
            document.Open();

            // Aggiungere contenuto al PDF
            document.Add(new Paragraph("Dettagli della Vendita"));
            document.Add(new Paragraph(" ")); // Aggiunge uno spazio vuoto

            var table = new PdfPTable(2); // Due colonne per etichette e valori
            table.AddCell("Codice Fiscale Cliente:");
            table.AddCell(vendita.CodiceFiscaleCliente);
            table.AddCell("Data Vendita:");
            table.AddCell(vendita.DataVendita.ToString("dd/MM/yyyy"));
            table.AddCell("Prodotto:");
            table.AddCell(vendita.Prodotto.Nome);
            table.AddCell("Numero Ricetta:");
            table.AddCell(vendita.NumeroRicetta ?? "N/A");
            table.AddCell("Quantita:");
            table.AddCell(vendita.Quantita.ToString());

            document.Add(table);
            document.Close();

            memoryStream.Position = 0;
            return File(memoryStream, "application/pdf", $"Vendita_{id}.pdf");
        }
    }
}
