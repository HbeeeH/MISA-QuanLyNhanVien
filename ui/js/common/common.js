// các hàm dùng chung toàn chương trình
var CommonFn = CommonFn || {};

// Hàm format số tiền
CommonFn.formatMoney = money => {
    if (money && !isNaN(money)) {
        return money.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
    } else {
        return money;
    }
}

CommonFn.formatMoney2 = (n, currency) => {
    if (currency) {
        return n.toFixed(2).replace(/./g, function (c, i, a) {
            return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
        }) + currency;
    } else {
        return n.toFixed(2).replace(/./g, function (c, i, a) {
            return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
        });
    }
}

CommonFn.formatMoney3 = money => {
    if (money && isNaN(money)) {
        return parseInt(money.split('.').join(''));
        // return parseInt(money.replace(/./g, ''));
    }
}

// Format ngày tháng
CommonFn.formatDate = dateSrc => {
    let date = new Date(dateSrc),
        year = date.getFullYear().toString(),
        month = (date.getMonth() + 1).toString().padStart(2, '0'),
        day = date.getDate().toString().padStart(2, '0');

    return `${year}-${month}-${day}T00:00:00`;
}

CommonFn.formatDateInput = dateSrc => {
    let date = new Date(dateSrc),
        year = date.getFullYear().toString(),
        month = (date.getMonth() + 1).toString().padStart(2, '0'),
        day = date.getDate().toString().padStart(2, '0');

    return `${year}-${month}-${day}`;
}

CommonFn.formatDateOutput = dateSrc => {
    let date = new Date(dateSrc),
        year = date.getFullYear().toString(),
        month = (date.getMonth() + 1).toString().padStart(2, '0'),
        day = date.getDate().toString().padStart(2, '0');

    return `${day}/${month}/${year}`;
}

// Hàm ajax gọi lên server lấy dữ liệu
CommonFn.Ajax = (url, method, data, fnCallBack, errorFnCallBack, async = true) => {
    $.ajax({
        url: url,
        method: method,
        async: async,
        data: JSON.stringify(data),
        headers: {
            "Content-Type": "application/json"
        },
        crossDomain: true,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            fnCallBack(response);
        },
        error: function (errormessage) {
            errorFnCallBack(errormessage.responseText);
            console.log(errormessage.responseText);
        }
    })
}

// Hàm gọi toast messenger
CommonFn.toast = (toast, mes, content) => {
    let main = $(`#${toast}`);
    if (main) {
        let toastMes = $('<div></div>'),
            delay = (mes.duration / 1000).toFixed(2);
        toastMes.addClass('messenger', 'img', `${mes.type}-mes`);
        toastMes.addClass('img');
        toastMes.addClass(`${mes.type}-mes`);
        toastMes.css('animation', `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`);
        toastMes.html(`<div class="mes-content">
                            ${content}
                        </div>
                        <div CommandType="close" class="close-btn-3"><i class="fas fa-times"></i></div>`);
        main.append(toastMes);

        setTimeout(function () {
            toastMes.remove();
        }, mes.duration + 1000);

        toastMes.find("[CommandType = 'close']").off("click");
        toastMes.find("[CommandType = 'close']").on("click", function () {
            toastMes.remove();
        });

    } else {
        console.log("Không xác định được toast messenger")
    }
}

