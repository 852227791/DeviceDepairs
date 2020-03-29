var DeptID = "0";
var ProfessionID = "0";//定义ID
var ClassID = "0";
firstFunction = function () {
    initDeptTree();
    initProfessionTable({ DeptID: DeptID });
    initClassTable({ ProfessionID: ProfessionID });
    bindCheckEvent();
    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editProfession", "#fSaveProfession", "../Profession/GetProfessionEdit", "#btnSaveProfession", "1", "#professionGrid", menuId, "1", "#ProfessionID");
    Easy.bindSaveButtonClickEvent("#editClass", "#fSaveClass", "../Class/GetClassEdit", "#btnSaveClass", "1", "#classGrid", menuId, "1", "#ClassID");
}

//加载树
initDeptTree = function () {
    $("#deptTree").tree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        onClick: function (node) {
            if (Easy.bindPowerValidationEvent(menuId, "1", "viewProfession")) {
                initProfessionTable({ DeptID: node.id });
            }
        }
    });
}

//加载表格
initProfessionTable = function (queryData) {
    $("#professionGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ProfessionID", hidden: true },
            { field: "Name", title: "专业名称", width: 300, sortable: true },
            { field: "EnglishName", title: "首字母", width: 60, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        url: "../Profession/GetProfessionList", sortName: "EnglishName", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        },
        onClickRow: function (index, row) {
            if (Easy.bindPowerValidationEvent(menuId, "2", "viewClass")) {
                initClassTable({ ProfessionID: row.ProfessionID });
            }
        }
    });
}

//加载表格
initClassTable = function (queryData) {
    $("#classGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ClassID", hidden: true },
            { field: "Name", title: "班级名称", width: 140, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        url: "../Class/GetClassList", sortName: "ClassID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "2"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };

    var row1 = {};
    row1.url = "../Profession/GetValidataProfessionName";
    row1.id = "#ProfessionID";
    row1.type = [{}];
    row1.type[0].typeId = "#DeptID";
    row1.type[0].type = "3";
    data.jsontext.push(row1);

    var row2 = {};
    row2.url = "../Class/GetValidataClassName";
    row2.id = "#ClassID";
    row2.type = [{}];
    row2.type[0].typeId = "#cProfessionID";
    row2.type[0].type = "3";
    data.jsontext.push(row2);

    Easy.checkValue(data);
}

//添加
addProfession = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "addProfession")) {
        var nodes = $("#deptTree").tree("getSelected");
        if (Easy.checkTreeGridRow(nodes, "校区")) {
            DeptID = nodes.id;
            ProfessionID = "0";
            Easy.OpenDialogEvent("#editProfession", "编辑专业", 400, 220, "../Profession/ProfessionEdit", "#editProfession-buttons");
        }
    }
}

//修改
editProfession = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "editProfession")) {
        var rows = $("#professionGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "专业")) {
            ProfessionID = rows[0].ProfessionID;
            Easy.OpenDialogEvent("#editProfession", "编辑专业", 400, 220, "../Profession/ProfessionEdit", "#editProfession-buttons");
        }
    }
}

//启用
enableProfession = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enableProfession")) {
        bindUpdateProfessionStatus("启用", "1");
    }
}

//停用
disableProfession = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disableProfession")) {
        bindUpdateProfessionStatus("停用", "2");
    }
}

bindUpdateProfessionStatus = function (confirmstr, status) {
    var rows = $("#professionGrid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "专业")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Profession/GetUpdateProfessionStatus", rows[0].ProfessionID, "1", "#professionGrid");
    }
}

//添加
addClass = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "addClass")) {
        var rows = $("#professionGrid").datagrid("getSelections");
        if (Easy.checkRow(rows, "专业")) {
            ProfessionID = rows[0].ProfessionID;
            ClassID = "0";
            Easy.OpenDialogEvent("#editClass", "编辑班级", 400, 220, "../Class/ClassEdit", "#editClass-buttons");
        }
    }
}

//修改
editClass = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "editClass")) {
        var rows = $("#classGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "班级")) {
            ClassID = rows[0].ClassID;
            Easy.OpenDialogEvent("#editClass", "编辑班级", 400, 220, "../Class/ClassEdit", "#editClass-buttons");
        }
    }
}

//启用
enableClass = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "enableClass")) {
        bindUpdateClassStatus("启用", "1");
    }
}

//停用
disableClass = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "disableClass")) {
        bindUpdateClassStatus("停用", "2");
    }
}

bindUpdateClassStatus = function (confirmstr, status) {
    var rows = $("#classGrid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "班级")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Class/GetUpdateClassStatus", rows[0].ClassID, "1", "#classGrid");
    }
}

