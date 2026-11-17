using Microsoft.AspNetCore.Mvc;
using UnstoreProjectADS.Core.Entidades;
using UnstoreProjectADS.Views.ViewModels;

namespace UnstoreProjectADS.Views.ViewComponents;

public class CardProdutoViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Produto produto)
    {
        return View(new ViewModelProduto(produto));
    }
}