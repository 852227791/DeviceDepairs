bindFormEvent = function () {
    $("#sGiveID").combobox({
        url: "../sGive/GetsItemsGiveCombobox?DeptID=" + DeptID,
        valueField: "id",
        textField: "text",
        panelHeight: 120,
        multiple: false
    });
    setTimeout(function () {
        $("#ItemID3").textbox("setValue", ItemID);
    }, 2);

};
bindFormEvent();