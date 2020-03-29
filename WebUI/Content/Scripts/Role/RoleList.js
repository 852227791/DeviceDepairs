var RoleID = "0";//定义ID
firstFunction = function () {
    //加载表格
    initTable({});

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editRole", "#fSave", "../Role/GetRoleEdit", "#btnSave", "1", "#grid", menuId, "1", "#RoleID");
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "RoleID", hidden: true },
            { field: "Name", title: "角色名", width: 100, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "RoleType", title: "角色分类", width: 70, sortable: true },
            { field: "Description", title: "角色描述", width: 300, sortable: true }
        ]],
        url: "../Role/GetRoleList", sortName: "RoleID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = {//得到用户输入的参数
            txtName: $("#txtName").val(),
            selStatus: "" + $("#selStatus").combobox("getValues"),
            selRoleType: "" + $("#selRoleType").combobox("getValues")
        }
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({});
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=1",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: "80",
        multiple: true
    });
    $("#selRoleType").combobox({
        url: "../Refe/SelList?RefeTypeID=2",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: "80",
        multiple: true
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        RoleID = "0";
        $("#btnSave").show();//显示保存按钮
        Easy.OpenDialogEvent("#editRole", "编辑角色", 650, 250, "../Role/RoleEdit", "#editRole-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "角色")) {
            RoleID = rows[0].RoleID;
            $("#btnSave").show();//显示保存按钮
            Easy.OpenDialogEvent("#editRole", "编辑角色", 600, 250, "../Role/RoleEdit", "#editRole-buttons");
        }
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "用户")) {
            RoleID = rows[0].RoleID;
            $("#btnSave").hide();//隐藏保存按钮
            Easy.OpenDialogEvent("#editRole", "编辑角色", 600, 250, "../Role/RoleEdit", "#editRole-buttons");
        }
    }
}

//启用
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
}

//绑定更新状态方法
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "角色")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Role/GetUpdateStatus", rows[0].RoleID, "1", "#grid");
    }
}