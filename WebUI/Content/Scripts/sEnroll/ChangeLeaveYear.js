bindFormEvent = function () {
    setTimeout(function () {

        $("#modlLeave").combobox({
            url: "../Common/LeaveYearCombobox",
            valueField: "id",
            queryParams: { startYear: "2010", isSelected: false },
            textField: "text",
            multiple: false,
            editable: false
        });
        $("#modlsEnrollsProfessionID").textbox("setValue", rowData.sEnrollsProfessionID);
        $("#modlName").html(rowData.StudentName);
        $("#modlIDCard").html(rowData.IDCard);
        $("#modlEnrollNum").html(rowData.EnrollNum);
        $("#modlMajor").html(rowData.Major);
        $("#modlLeave").combobox("setValue", rowData.LeaveYear);
    }, 1);
};
bindFormEvent();
