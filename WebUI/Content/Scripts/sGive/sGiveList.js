var sGiveID = "0";
firstFunction = function () {
    initDeptTree();//加载树
    bindGive(0);//初始化主体配品
    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editsGive", "#fSave", "../sGive/GetsGiveEdit", "#btnSave", "1", "#grid", menuId, "1", "#sGiveID");
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        sGiveID = "0";
        var node = $("#deptTree").tree("getSelected");//选中的所有ID
        //验证是否选中行
        if (Easy.checkNode(node, "校区")) {
            var isLeaf = $("#deptTree").tree('isLeaf', node.target);
            if (!isLeaf) {
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                Easy.OpenDialogEvent("#editsGive", "添加配品", 650, 280, "../sGive/sGiveEdit", "#editsGive-buttons");
            }
        }
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "配品")) {
            sGiveID = rows[0].sGiveID;
            Easy.OpenDialogEvent("#editsGive", "编辑配品", 650, 280, "../sGive/sGiveEdit", "#editsGive-buttons");
        }
    }
}

//启用
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdatesGiveStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdatesGiveStatus("停用", "2");
    }
}

//加载树
initDeptTree = function () {
    $("#deptTree").tree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1,2" },
        animate: true,
        lines: true,
        onClick: function (node) {
            if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
                bindGive(node.id);
            }
        }
    });
}

//查看主体的配品列表
bindGive = function (deptId) {
    //加载表格
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: { MenuID: menuId, DeptID: deptId },//异步查询的参数
        columns: [[
            { field: "sGiveID", checkbox: true },
            { field: "Name", title: "配品名称", width: 120, sortable: true },
            { field: "Money", title: "配品金额", width: 120, sortable: true, halign: 'left', align: 'right' },
            { field: "Remark", title: "备注", width: 300, sortable: true }
        ]],
        url: "../sGive/GetsGiveList", sortName: "sGiveID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.Status === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定更新状态方法
bindUpdatesGiveStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "配品")) {
        //批量修改状态
        Easy.bindUpdateValue(confirmstr, status, "../sGive/GetUpdatesStatus", rows[0].sGiveID, "1", "#grid");
    }
}