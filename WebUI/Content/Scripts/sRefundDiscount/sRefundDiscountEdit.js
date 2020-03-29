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