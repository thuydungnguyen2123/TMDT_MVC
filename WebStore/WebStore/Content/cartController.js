var cart = {
    init:function()
    {
        cart.regEvents();
    },
    regEvents:function()
    {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/danh-sach-san-pham";
        });
        $('#btnOrder').off('click').on('click', function () {
            var shipFee = $('[name="radio"]:radio:checked').val();
            var kt = document.getElementById('giohang').innerText;
            if (kt == "$0") {
                alert("Chưa có sản phẩm trong giỏ hàng!");
            }
            else if (shipFee == null) {
                alert("Hãy chọn dịch vụ giao hàng!");
            }
            else {
                window.location.href = "/dat-hang?ship=" + shipFee;
            }
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    product: {
                        Ma_Product: $(item).data('id')
                    }
                });
            });
            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

        $('#btnClearAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/ClearAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/Delete',
                data:{id:$(this).data('id')},
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

    }
}
cart.init();