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
        Easy.OpenDialogEvent("#chooseFee", "选择收费项", 800, 600, "../Fee/ChooseFee", "#chooseFee-buttons");
    });
    $("#btnSaveChooseFee").click(function () {
        var rows = $("#choosefeegrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费项")) {
            $("#FeeName").textbox("setValue", rows[0].Name + "_" + rows[0].VoucherNum + "_" + rows[0].DetailName);
            $("#FeeDetailID").textbox("setValue", rows[0].FeeDetailID);
            $("#chooseFee").dialog('close');
        }
    });


}
bindFormEvent();
bindSelectInfo = function (refundId) {
    if (refundId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Refund/SelectRefund", refundId);
            var r = JSON.parse(result.Message)[0];
            $("#FeeDetailID").textbox("setValue", r.FeeDetailID);
            $("#FeeName").textbox("setValue", r.FeeName);
            $("#RefundID").textbox("setValue", r.RefundID);
            $("#Sort").combobox("setValue", r.Sort);
            $("#RefundMoney").numberspinner("setValue", r.RefundMoney);
            $("#RefundTime").datebox("setValue", r.RefundTime);
            $("#PayObject").textbox("setValue", r.PayObject);
            $("#Remark").textbox("setValue", r.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}
bindSelectInfo(RefundID);