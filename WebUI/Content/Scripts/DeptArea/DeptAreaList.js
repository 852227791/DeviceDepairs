var DeptID = "0";
var deptAreaId = "0";
firstFunction = function () {
    initDeptTree();
    initGrid();
    Easy.bindSaveButtonClickEvent("#editdeptArea", "#fSave", "../DeptArea/GetDeptAreaEdit", "#btnSave", "1", "#grid", menuId, "1", "#DeptAreaID");
};

initDeptTree = function () {
    $("#deptTree").tree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        onSelect: function (node) {
            DeptID = node.id;
           
            $("#grid").datagrid({
                url: "../DeptArea/GetDeptAreaList",
                queryParams: { DeptID: DeptID }
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
            { field: "DeptAreaID", hidden: true },
            { field: "Name", title: "收费校区名称", width: 150, sortable: true },
            { field: "Queue", title: "排序", width: 70, sortable: true },
            { field: "Status", title: "状态", width: 70, sortable: true }
        ]], sortName: "DeptAreaID", sortOrder: "asc",
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
            deptAreaId = "0";
            Easy.OpenDialogEvent("#editdeptArea", "编辑收费校区", 350, 200, "../DeptArea/DeptAreaEdit", "#editdeptArea-buttons");
        }
    }
};
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费校区信息")) {
            deptAreaId = rows[0].DeptAreaID;
            Easy.OpenDialogEvent("#editdeptArea", "编辑收费校区", 350, 200, "../DeptArea/DeptAreaEdit", "#editdeptArea-buttons");
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
    if (Easy.checkRows(rows, "收费校区")) {
        Easy.bindUpdateValue(confirmstr, status, "../DeptArea/GetUpdateStatus", rows[0].DeptAreaID, "1", "#grid");
    }
}