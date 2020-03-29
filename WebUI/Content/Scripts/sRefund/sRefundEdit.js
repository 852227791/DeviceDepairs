bindFormEvent = function () {
    $("#Sort").combobox({
        url: "../Refe/SelList?RefeTypeID=10",
        valueField: "Value",
        textField: "RefeName",
        multiple: false,
        editable: false
    });
    $("#RefundTime").val(Easy.GetDateTimeNow());
    $("#btnChooseFee").click(function () {
        Easy.OpenDialogEvent("#choosesFee", "选择收费项", 800, 600, "../sFeesOrder/ChoosesFeeOrderList", "#choosesFee-buttons");
    });
    $("#btnSaveChoosesFee").click(function () {
        var rows = $("#choosesfeegrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费项")) {
            $("#sFeeName").textbox("setValue", rows[0].Name + "_" + rows[0].VoucherNum + "_" + rows[0].Content + "_" + rows[0].DetailName);
            $("#sFeesOrderID").textbox("setValue", rows[0].sFeesOrderID);
            $("#choosesFee").dialog('close');
        }
    });
}
bindFormEvent();
bindSelectInfo = function (refundId) {
    if (refundId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../sRefund/SelectsRefund", refundId);
            var r = JSON.parse(result.Data)[0];
            $("#sFeesOrderID").textbox("setValue", r.sFeesOrderID);
            $("#sFeeName").textbox("setValue", feeName);
            $("#sRefundID").textbox("setValue", r.sRefundID);
            $("#Sort").combobox("setValue", r.Sort);
            $("#RefundMoney").numberspinner("setValue", r.RefundMoney);
            $("#RefundTime").datebox("setValue", r.RefundTime);
            $("#PayObject").textbox("setValue", r.PayObject);
            $("#Remark").textbox("setValue", r.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}
bindSelectInfo(sRefundId);