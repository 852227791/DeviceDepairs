bindFormEvent = function () {
    setTimeout(function () {

        $("#modStatus").combobox({
            url: "../Refe/SelList?RefeTypeID=21",
            valueField: "Value",
            textField: "RefeName",
            multiple: false,
            editable: false
        });
        $("#modsEnrollsProfessionID").textbox("setValue", rowData.sEnrollsProfessionID);
        $("#modName").html(rowData.StudentName);
        $("#modIDCard").html(rowData.IDCard);
        $("#modEnrollNum").html(rowData.EnrollNum);
        $("#modMajor").html(rowData.Major);

    }, 1);
};
bindFormEvent();