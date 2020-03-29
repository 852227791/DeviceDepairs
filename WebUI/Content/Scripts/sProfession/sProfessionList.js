var DeptID = "0";
var sProfessionID = "0";
firstFunction = function () {
    initDeptTree();
    initGrid();
    Easy.bindSaveButtonClickEvent("#editsProfession", "#fSave", "../sProfession/GetsProfessionEdit", "#btnSave", "1", "#grid", menuId, "1", "#sProfessionID");
    bindSerchForm();
};

initDeptTree = function () {
    $("#deptTree").tree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        onSelect: function (node) {
            DeptID = node.id;
            var year = $("#selYear").combobox("getValue");
            var month = $("#selMonth").combobox("getValue");

            $("#grid").datagrid({
                url: "../sProfession/GetsProfessionList",
                queryParams: { DeptID: DeptID, MenuID: menuId, Month: month, Year: year }
            });
        }
    });
};
initGrid = function (deptId) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        showFooter: true,
        columns: [[
            { field: "sProfessionID", hidden: true },
            { field: "Name", title: "专业名称", width: 300, sortable: true },
            { field: "Year", title: "年份", width: 70, sortable: true },
            { field: "Month", title: "月份", width: 70, sortable: true },
            { field: "Status", title: "状态", width: 70, sortable: true }
        ]], sortName: "sProfessionID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
};

add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        var nodes = $("#deptTree").tree("getSelected");
        if (Easy.checkTreeGridRow(nodes, "校区")) {
            sProfessionID = "0";
            Easy.OpenDialogEvent("#editsProfession", "编辑报名专业", 500, 500, "../sProfession/sProfessionEdit", "#editsProfession-buttons");
        }
    }
};
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "招生专业信息")) {
            sProfessionID = rows[0].sProfessionID;
            Easy.OpenDialogEvent("#editsProfession", "编辑报名专业", 500, 500, "../sProfession/sProfessionEdit", "#editsProfession-buttons");
        }
    }
};

disable = function () {
    bindUpdateStatus("停用", "2");
};

enable = function () {
    bindUpdateStatus("启用", "1");
};

bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");
    if (Easy.checkRows(rows, "招生专业")) {
        Easy.bindUpdateValue(confirmstr, status, "../sProfession/GetUpdateStatus", rows[0].sProfessionID, "1", "#grid");
    }
}

bindSerchForm = function () {
    $("#selMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=16",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: false,
        value: 0,
        onSelect: function (record) {
            var year = $("#selYear").combobox("getValue");
            $("#grid").datagrid({
                url: "../sProfession/GetsProfessionList",
                queryParams: { DeptID: DeptID, MenuID: menuId, Year: year, Month: record.Value }
            });
        }
    });
    $("#selYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=Yes",
        valueField: "Id",
        textField: "Text",
        panelHeight: 80,
        multiple: false,
        onSelect: function (record) {
            var month = $("#selMonth").combobox("getValue");
            $("#grid").datagrid({
                url: "../sProfession/GetsProfessionList",
                queryParams: { DeptID: DeptID, MenuID: menuId, Year: record.Id, Month: month }
            });
        }
    });
};

