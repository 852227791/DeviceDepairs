bindFormEvent = function () {
    $("#Year").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 80,
        multiple: false
    });

    $("#Month").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false
    });
    setTimeout(function () {
        $("#DeptID").textbox("setValue", DeptID);
    }, 1);
}

bindSelectInfo = function () {
    if (sProfessionID != "0") {
        var result = Easy.bindSelectInfo("../sProfession/SelectsProfession", sProfessionID);
        var s = JSON.parse(result.Data)[0];
        setTimeout(function () {
            $("#DeptID").textbox("setValue", s.DeptID);
            $("#sProfessionID").textbox("setValue", s.sProfessionID);
            $("#Year").combobox("setValue", s.Year);
            $("#Month").combobox("setValue", s.Month);
            $("#ProfessionID").combogrid({
                value: s.ProfessionID,
                url: "../Profession/ProfessionCombobox",
                queryParams: { DeptID: DeptID },
                idField: "Value",
                textField: "Text",
                panelWidth: 350,
                multiple: false,
                columns: [[
                    { field: 'Value', checkbox: false },
                    { field: 'Text', title: '专业名称', width: 300, sortable: true }
                ]]
            });
        }, 1);
     
    }
    else {
        $("#ProfessionID").combogrid({
            url: "../Profession/ProfessionCombobox",
            queryParams: { DeptID: DeptID },
            idField: "Value",
            textField: "Text",
            panelWidth: 350,
            multiple: true,
            columns: [[
                { field: 'Value', checkbox: true },
                { field: 'Text', title: '专业名称', width: 300, sortable: true }
            ]]
        });
    }
};
bindSelectInfo();
bindFormEvent();