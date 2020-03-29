bindFormEvent = function () {
    var nodes = $("#itemTree").treegrid("getSelected");
    $("#sProfessionID").combogrid({
        delay:500,
        url: "../sProfession/GetsItemProfessionCombobox",
        idField: "id",
        textField: "name",
        loadMsg: "数据加载中...",
        panelWidth: 350,
        queryParams: { DeptID: DeptID, Year: nodes.Year, Month: nodes.Month },
        multiple: true,
        columns: [[
            { field: 'id', checkbox: true },
            { field: 'name', title: '专业名称', width: 300, sortable: true }
        ]], sortName: "EnglishName", sortOrder: "asc"
    });
    setTimeout(function () {
        $("#ItemID2").textbox("setValue", ItemID);
    }, 1);
};
bindFormEvent();