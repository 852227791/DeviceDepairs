var queryDataDefault = { selStatus: "1" };
var userId = "";
var userType = "";
var roleId = "";
firstFunction = function () {
    //加载表格
    initTable(queryDataDefault);

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //加载插入用户角色事件
    bindInsertUserRoleEvent();

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#ChooseDept", "#fSave", "../User/InsertPurview", "#btnSave", "1", "#deptGrid", menuId, "2", "#PurviewID");
}

//加载表格
initTable = function (queryData) {
    $("#userGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "UserID", hidden: true },
            { field: "LoginName", title: "登录名", width: 100, sortable: true },
            { field: "Name", title: "姓名", width: 100, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "UserType", title: "用户分类", width: 70, sortable: true },
            { field: "Sex", title: "性别", width: 60, sortable: true },
            { field: "Mobile", title: "手机", width: 100, sortable: true },
            { field: "LoginTime", title: "最后登录时间", width: 140, sortable: true }
        ]],
        url: "../User/GetUserList", sortName: "UserID", sortOrder: "desc",
        onSelect: function (rowIndex, rowData) {
            userId = rowData.UserID;
            userType = rowData.UserTypeValue;
            if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
                initRoleTable({ UserID: userId });
            }
            initDeptTable({ UserID: 0, RoleID: 0 });
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = {//得到用户输入的参数
            txtLoginName: $("#txtLoginName").val(),
            txtName: $("#txtName").val(),
            selUserType: "" + $("#selUserType").combobox("getValues"),
            selSex: "" + $("#selSex").combobox("getValues"),
            txtMobile: $("#txtMobile").val(),
            txtLoginTimeS: $("#txtLoginTimeS").datebox("getValue"),
            txtLoginTimeE: $("#txtLoginTimeE").datebox("getValue"),
            selStatus: "1"
        }
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable(queryDataDefault);
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#selUserType").combobox({
        url: "../Refe/SelList?RefeTypeID=2",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: "80",
        multiple: true
    });
    $("#selSex").combobox({
        url: "../Refe/SelList?RefeTypeID=3",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: "80",
        multiple: true
    });
    $("#txtLoginTimeS").datebox({});
    $("#txtLoginTimeE").datebox({});
}

//加载表格
initRoleTable = function (queryData) {
    $("#roleGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "RoleID", hidden: true },
            { field: "Name", title: "角色名", width: 100, sortable: true },
            { field: "RoleType", title: "角色分类", width: 70, sortable: true },
            { field: "Description", title: "角色描述", width: 300, sortable: true }
        ]],
        url: "../User/GetUserRoleList", sortName: "RoleID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        onSelect: function (rowIndex, rowData) {
            roleId = rowData.RoleID;
            if (Easy.bindPowerValidationEvent(menuId, "2", "view")) {
                initDeptTable({ UserID: userId, RoleID: roleId });
            }
        }
    });
}

//加载表格
initDeptTable = function (queryData) {
    $("#deptGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptID", hidden: true },
            { field: "Name", title: "部门名", width: 150, sortable: true },
            { field: "Range", title: "权限范围", width: 70, sortable: true }
        ]],
        url: "../User/GetPurviewList", sortName: "DeptID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "2")
    });
}

addRole = function () {
    var userRows = $("#userGrid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(userRows, "用户")) {
        if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
            $("#btnSave").show();//显示保存按钮
            Easy.OpenDialogEvent("#ChooseRole", "选择角色", 600, 400, "../Role/ChooseRole", "#chooseRole-buttons");
        }
    }
}

delRole = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "del")) {
        var roleRows = $("#roleGrid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(roleRows, "角色")) {
            $.messager.confirm("系统消息", "您确认要删除吗？", function (r) {
                if (r) {
                    $.ajax({
                        type: "post",
                        url: "../User/DeleteUserRole",
                        async: false,
                        data: { UserRoleID: roleRows[0].UserRoleID },
                        dataType: "json",
                        success: function (result) {
                            if (result.IsError === false) {
                                Easy.centerShow("系统消息", "删除成功", 3000);
                                $("#roleGrid").datagrid("load");//刷新表格
                            }
                            else {
                                Easy.centerShow("系统消息", result.Message, 3000);
                            }
                        },
                        error: function () {
                            Easy.centerShow("系统消息", "删除失败", 3000);
                        }
                    });
                }
            });
        }
    }
}

addDept = function () {
    var userRows = $("#userGrid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(userRows, "用户")) {
        var roleRows = $("#roleGrid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(roleRows, "角色")) {
            if (Easy.bindPowerValidationEvent(menuId, "2", "add")) {
                $("#btnSave").show();//显示保存按钮
                Easy.OpenDialogEvent("#ChooseDept", "选择数据权限", 500, 180, "../Dept/ChooseDept", "#ChooseDept-buttons");
            }
        }
    }
}
delDept = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "del")) {
        var userRows = $("#userGrid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(userRows, "用户")) {
            var deptRows = $("#deptGrid").datagrid("getSelections");//选中的所有ID
            //验证是否选中行
            if (Easy.checkRow(deptRows, "数据权限")) {
                $.messager.confirm("系统消息", "您确认要删除吗？", function (r) {
                    if (r) {
                        $.ajax({
                            type: "post",
                            url: "../User/DeletePurview",
                            async: false,
                            data: { PurviewID: deptRows[0].PurviewID },
                            dataType: "json",
                            success: function (result) {
                                if (result.IsError === false) {
                                    Easy.centerShow("系统消息", "删除成功", 3000);
                                    $("#deptGrid").datagrid("load");//刷新表格
                                }
                                else {
                                    Easy.centerShow("系统消息", result.Message, 3000);
                                }
                            },
                            error: function () {
                                Easy.centerShow("系统消息", "删除失败", 3000);
                            }
                        });
                    }
                });
            }
        }
    }
}

//绑定插入用户角色事件
bindInsertUserRoleEvent = function () {
    $("#btnSave-Choose").click(function () {
        if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
            var rows_Choose = $("#grid-Choose").datagrid("getSelections");//选中的所有ID
            //验证是否选中行
            if (Easy.checkRow(rows_Choose, "角色")) {
                $.ajax({
                    type: "post",
                    url: "../User/InsertUserRole",
                    async: false,
                    data: { UserID: userId, RoleID: rows_Choose[0].RoleID },
                    dataType: "json",
                    success: function (result) {
                        if (result.IsError === false) {
                            $("#btnSave-Choose").linkbutton("disable");//禁用按钮
                            $("#ChooseRole").dialog("close");//关闭弹窗
                            Easy.centerShow("系统消息", "保存成功", 3000);
                            $("#roleGrid").datagrid("load");//刷新表格
                            $("#btnSave-Choose").linkbutton("enable");//解除按钮禁用
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 3000);
                        }
                    },
                    error: function () {
                        Easy.centerShow("系统消息", "保存失败", 3000);
                    }
                });
            }
        }
    });
}