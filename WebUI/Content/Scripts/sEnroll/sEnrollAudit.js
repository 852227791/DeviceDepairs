bindFormEvent = function () {
    $("#audsEnrollTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });
    setTimeout(function () {
        $("#audId").textbox("setValue", rowData.sEnrollsProfessionID);
        $("#audName").textbox("setValue", rowData.StudentName);
        $("#audIdCard").textbox("setValue", rowData.IDCard);
    }, 1);
};
bindFormEvent();