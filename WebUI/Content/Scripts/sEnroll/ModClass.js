bindFormEvent = function () {
    setTimeout(function () {

        $("#modcClass").combobox({
            url: "../Class/ClassCombobox",
            valueField: "Value",
            queryParams: { ProfessionID: rowData.ProfessionID },
            textField: "Text",
            multiple: false,
            editable: false
        });
        $("#modcsEnrollsProfessionID").textbox("setValue", rowData.sEnrollsProfessionID);
        $("#modcName").html(rowData.StudentName);
        $("#modcIDCard").html(rowData.IDCard);
        $("#modcEnrollNum").html(rowData.EnrollNum);
        $("#modcMajor").html(rowData.Major);
        $("#modcClass").combobox("setValue", rowData.ClassID);
    }, 1);
};
bindFormEvent();
