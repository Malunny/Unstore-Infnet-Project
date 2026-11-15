using Microsoft.AspNetCore.Mvc;
using UnstoreProjectADS.Models;
using UnstoreProjectADS.ViewModels;

namespace UnstoreProjectADS.ViewComponents;

public class CardProdutoViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Produto produto)
    {
        return View(new ViewModelProduto(produto));
    }
}