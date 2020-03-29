bindFormEvent = function () {
    $("#Sort").combobox({
        url: "../Refe/SelList?RefeTypeID=10",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#RefundTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });

    $("#btnChooseiFee").click(function () {
        Easy.OpenDialogEvent("#chooseiFee", "选择收费信息", 800, 600, "../iRefund/ChooseiFeeList", "#chooseiFee-buttons");
    });

    $("#btnSaveiFee").click(function () {
        var rows = $("#choosefeegrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费项")) {
            $("#FeeName").textbox("setValue", rows[0].Name + "_" + rows[0].VoucherNum + "_" + rows[0].FeeContent);
            $("#iFeeID").textbox("setValue", rows[0].iFeeID);
            $("#chooseiFee").dialog('close');
        }
    });
}

bindSelectInfo = function (iRefundId) {
    if (iRefundId != "0") {
        setTimeout(function () {
            var result1 = Easy.bindSelectInfo("../iRefund/SelectiRefund", iRefundId);
            var fee = JSON.parse(result1.Message)[0];
            $("#iRefundID").textbox("setValue", fee.iRefundID);
            $("#iFeeID").textbox("setValue", fee.iFeeID);
            $("#Sort").combobox("setValue", fee.Sort);
            $("#FeeName").textbox("setValue", fee.FeeName);
            $("#RefundMoney").numberspinner("setValue", fee.RefundMoney);
            $("#RefundTime").datebox("setValue", fee.RefundTime);
            $("#PayObject").textbox("setValue", fee.PayObject);
            $("#Remark").textbox("setValue", fee.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

bindFormEvent();

bindSelectInfo(iRefundID);
