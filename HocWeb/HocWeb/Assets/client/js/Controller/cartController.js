var cart = {
    init: function () {
        cart.regEvents();

    },
    regEvents: function () {
        $(document).ready(function () {
            $('#btnContinue').click(function () {
                window.location.href = "/";
            });
        });
        $(document).ready(function () {
            $('#btnBuy').click(function () {
                window.location.href = "/thanh-toan";
            });
        });
        $(document).ready(function () {
            $('#btnUpdate').off('click').on('click', function () {
                var listPro = $('.txtQuantity');
                var carlist = [];
                $.each(listPro, function (i, item) {
                    carlist.push({
                        Quantity: $(this).val(),
                        product: {
                            ID: $(item).data('id')
                        }
                    });
                });
                $.ajax({
                    url: '/GioHang/Update',
                    data: { cartModel: JSON.stringify(carlist) },
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/gio-hang"
                        }
                    }
                })
            });
        });
        $(document).ready(function () {
            $('#btnDelete').click(function () {
                $.ajax({
                    url: '/GioHang/DelAll',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/gio-hang"
                        }
                    }
                })
            });

        })
        $(document).ready(function () {
            $('.remove').off('click').on('click', function () {
                $.ajax({
                    data: { id: $(this).data('id') },
                    url: '/GioHang/Del',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/gio-hang"
                        }
                    }
                })
            });
        });
    }
}
cart.init();