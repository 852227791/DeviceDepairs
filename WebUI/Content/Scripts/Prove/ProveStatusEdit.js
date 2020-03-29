//绑定表单
bindFormEvent = function () {
    $("#Status").combobox({
        url: "../Refe/SelList?RefeTypeID=9",
        valueField: "Value",
        textField: "RefeName",
        editable: false,
        value: 1
    });
}

//绑定显示信息的方法
bindSelectInfo = function (proveId) {
    if (proveId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Prove/SelectProve", proveId);
            var prove = JSON.parse(result.Message)[0];
            $("#sProveID").textbox("setValue", prove.ProveID);
            $("#Status").combobox("setValue", prove.Status);
        }, 1);
    }
}

//加载表单
bindFormEvent();

bindSelectInfo(ProveID);//加载数据

