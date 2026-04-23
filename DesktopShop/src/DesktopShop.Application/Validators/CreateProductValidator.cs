using FluentValidation;
using DesktopShop.Application.DTOs.Product;

namespace DesktopShop.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
            .MaximumLength(200).WithMessage("Tên sản phẩm tối đa 200 ký tự.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Phải chọn danh mục.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Số lượng tồn kho không được âm.");
    }
}
