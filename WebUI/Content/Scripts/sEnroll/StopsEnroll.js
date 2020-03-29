bindStopForm = function () {
    var row = $("#grid").datagrid("getSelected");

    setTimeout(function () {
        $("#StopsEnrollsProfessionID").textbox("setValue", row.sEnrollsProfessionID);
    }, 1);

    $("#StopSort").combobox({
        url: "../Refe/SelList?RefeTypeID=22",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

bindStopForm();