var UserID = "0";//定义ID
firstFunction = function () {
    //加载表格
    initTable({});

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editUser", "#fSave", "../User/GetUserEdit", "#btnSave", "1", "#grid", menuId, "1", "#UserID");
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
            txtLoginName: $("#txtLoginName").val(),
            txtName: $("#txtName").val(),
            selStatus: "" + $("#selStatus").combobox("getValues"),
            selUserType: "" + $("#selUserType").combobox("getValues"),
            selSex: "" + $("#selSex").combobox("getValues"),
            txtMobile: $("#txtMobile").val(),
            txtLoginTimeS: $("#txtLoginTimeS").datebox("getValue"),
            txtLoginTimeE: $("#txtLoginTimeE").datebox("getValue")
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
        panelHeight: 80,
        multiple: true
    });
    $("#selUserType").combobox({
        url: "../Refe/SelList?RefeTypeID=2",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: true
    });
    $("#selSex").combobox({
        url: "../Refe/SelList?RefeTypeID=3",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 80,
        multiple: true
    });
    $("#txtLoginTimeS").datebox({});
    $("#txtLoginTimeE").datebox({});
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        UserID = "0";
        $("#btnSave").show();//显示保存按钮
        Easy.OpenDialogEvent("#editUser", "编辑用户", 650, 280, "../User/UserEdit", "#editUser-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "用户")) {
            UserID = rows[0].UserID;
            $("#btnSave").show();//显示保存按钮
            Easy.OpenDialogEvent("#editUser", "编辑用户", 650, 280, "../User/UserEdit", "#editUser-buttons");
        }
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "用户")) {
            UserID = rows[0].UserID;
            $("#btnSave").hide();//隐藏保存按钮
            Easy.OpenDialogEvent("#editUser", "编辑用户", 600, 280, "../User/UserEdit", "#editUser-buttons");
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

//重置密码
reset = function () {
    bindResetPassword("重置密码", "123456");
}

//绑定更新状态方法
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "用户")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../User/GetUpdateStatus", rows[0].UserID, "1", "#grid");
    }
}

//绑定重置密码方法
bindResetPassword = function (confirmstr, password) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "用户")) {
        //重置密码
        Easy.bindUpdateValue(confirmstr, password, "../User/GetResetPassword", rows[0].UserID, "1", "#grid");
    }
}