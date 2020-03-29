var RefeTypeID = "0";
var RefeID = "0";
firstFunction = function () {
    //加载表格
    initTable();

    //验证基础信息名是否重复
    bindCheckEvent();

    //点击基础分类添加保存按钮
    Easy.bindSaveButtonClickEvent("#editRefeType", "#fRefeTypeSave", "../Refe/GetRefeTypeEdit", "#btnRefeTypeSave", "1", "#refeTypeGrid", menuId, "1", "#RefeTypeID");

    //点击基础明细添加保存按钮
    Easy.bindSaveButtonClickEvent("#editRefe", "#fRefeSave", "../Refe/GetRefeEdit", "#btnRefeSave", "1", "#refeGrid", menuId, "2", "#RefeID");
}

//加载表格
initTable = function () {
    $("#refeTypeGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        columns: [[
            { field: "RefeTypeID", title: "ID", width: 30, sortable: true, halign: "left", align: "right" },
            { field: "TypeName", title: "分类名", width: 100, sortable: true },
            { field: "ModuleName", title: "模块名", width: 100, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        url: "../Refe/GetRefeTypeList", sortName: "RefeTypeID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        },
        onClickRow: function (index, row) {
            //加载明细
            $("#refeGrid").datagrid({
                url: "../Refe/GetRefeList?RefeTypeID=" + row.RefeTypeID
            });
        }
    });
    $("#refeGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        columns: [[
            { field: "RefeID", title: "ID", width: 30, sortable: true, halign: "left", align: "right" },
            { field: "RefeName", title: "名称", width: 100, sortable: true },
            { field: "Value", title: "值", width: 45, sortable: true, halign: "left", align: "right" },
            { field: "Queue", title: "排序", width: 45, sortable: true, halign: "left", align: "right" },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        sortName: "Queue", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "2"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

//添加
addRefeType = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        RefeTypeID = "0";
        $("#btnSave").show();//显示保存按钮
        Easy.OpenDialogEvent("#editRefeType", "编辑基础分类", 650, 250, "../Refe/RefeTypeEdit", "#editRefeType-buttons");
    }
}

//修改
editRefeType = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#refeTypeGrid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "基础分类")) {
            RefeTypeID = rows[0].RefeTypeID;
            $("#btnSave").show();//显示保存按钮
            Easy.OpenDialogEvent("#editRefeType", "编辑基础分类", 650, 250, "../Refe/RefeTypeEdit", "#editRefeType-buttons");
        }
    }
}

//启用
enableRefeType = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateRefeTypeStatus("启用", "1");
    }
}

//停用
disableRefeType = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateRefeTypeStatus("停用", "2");
    }
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Refe/CheckRefeTypeName";
    row1.id = "#RefeTypeID";
    row1.type = [];
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Refe/CheckRefeName";
    row2.id = "#RefeID";
    row2.type = [{}];
    row2.type[0].typeId = "#RefeType";
    row2.type[0].type = "1";
    data.jsontext.push(row2);
    var row3 = {};
    row3.url = "../Refe/CheckRefeValue";
    row3.id = "#RefeID";
    row3.type = [{}];
    row3.type[0].typeId = "#RefeType";
    row3.type[0].type = "1";
    data.jsontext.push(row3);
    Easy.checkValue(data);
}

//绑定更新状态方法
bindUpdateRefeTypeStatus = function (confirmstr, status) {
    var rows = $("#refeTypeGrid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "基础分类")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Refe/GetUpdateRefeTypeStatus", rows[0].RefeTypeID, "1", "#refeTypeGrid");
    }
}

//添加
addRefe = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "add")) {
        var rows = $("#refeTypeGrid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "基础分类")) {
            RefeTypeID = rows[0].RefeTypeID;
            RefeID = "0";
            $("#btnSave").show();//显示保存按钮
            Easy.OpenDialogEvent("#editRefe", "编辑基础明细", 650, 320, "../Refe/RefeEdit", "#editRefe-buttons");
        }
    }
}

//修改
editRefe = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "edit")) {
        var rows = $("#refeGrid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "基础明细")) {
            RefeID = rows[0].RefeID;
            $("#btnSave").show();//显示保存按钮
            Easy.OpenDialogEvent("#editRefe", "编辑基础明细", 650, 320, "../Refe/RefeEdit", "#editRefe-buttons");
        }
    }
}

//启用
enableRefe = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "enable")) {
        bindUpdateRefeStatus("启用", "1");
    }
}

//停用
disableRefe = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "disable")) {
        bindUpdateRefeStatus("停用", "2");
    }
}

//绑定更新状态方法
bindUpdateRefeStatus = function (confirmstr, status) {
    var rows = $("#refeGrid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "基础明细")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Refe/GetUpdateRefeStatus", rows[0].RefeID, "1", "#refeGrid");
    }
}