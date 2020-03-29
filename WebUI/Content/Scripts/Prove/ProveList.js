var ProveID = "0";//定义ID
var selectRow;
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);

    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editProve", "#fSave", "../Prove/GetProveEdit", "#btnSave", "1", "#grid", menuId, "1", "#ProveID");
    Easy.bindSaveButtonClickEvent("#editProveStatus", "#fStatusSave", "../Prove/GetUpdateStatus", "#btnSaveStatus", "1", "#grid", menuId, "1", "#ProveID");
    Easy.bindSaveUploadFile("#uploadProveStatus", "#fUpload", "../Prove/UploadProve", "#btnUploadStatus", "1", "#grid", menuId, "1", "upload");
    Easy.bindSaveUploadFile("#upProve", "#fupOldProve", "../Prove/UploadOldProve", "#btnupProve", "1", "#grid", menuId, "1", "uploadOldProve");
    bindSaveUplodMethod();
    Easy.bindSaveButtonClickEvent("#editProveNum", "#fPrintNumSave", "../Prove/GetProveNumEdit", "#btneditProveNum", "1", "#grid", menuId, "1", "#NProveID");
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
            { field: "ProveID", hidden: true },
            { field: "DeptName", title: "校区", width: 140, sortable: true },
            { field: "ProfessionName", title: "专业", width: 140, sortable: true },
            { field: "ClassName", title: "班级", width: 120, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "Mobile", title: "手机号", width: 100, sortable: true },
            { field: "ItemName", title: "证书名称", width: 140, sortable: true },
            { field: "ProveNum", title: "证书编号", width: 100, sortable: true },
            {
                field: "ShouldMoney", title: "实收金额", sortable: true,
                formatter: function (value, row, index) {
                    if (row.StatusValue === "9" || row.StatusValue === "1") {
                        return "0.00";
                    } else {
                        return value;
                    }

                }
            },
            {
                field: "RefundMoney", title: "核销金额", sortable: true
            },
            { field: "EnrollTime", title: "报名时间", width: 130, sortable: true },
            { field: "Status", title: "状态", width: 90, sortable: true }
        ]],
        url: "../Prove/GetProveList", sortName: "ProveID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "9") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=9",
        valueField: "Value",
        textField: "RefeName",
        panelWidth: 100,
        multiple: true
    });
    $("#treeDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
    $("#txtEnrollTimeS").datebox({});
    $("#txtEnrollTimeE").datebox({});
}

rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtProveName: $("#txtProveName").val(),
        txtName: $("#txtName").val(),
        txtIDCard: $("#txtIDCard").val(),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        treeDept: $("#treeDept").combotree("getValue"),
        txtEnrollTimeS: $("#txtEnrollTimeS").datebox("getValue"),
        txtEnrollTimeE: $("#txtEnrollTimeE").datebox("getValue")
    }
    return queryData;
}

//导入证书状态
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#uploadProveStatus", "变更证书状态", 410, 320, "../Prove/ProveUp", "#uploadProveStatus-buttons");
    }
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        ProveID = "0";
        Easy.OpenDialogEvent("#editProve", "编辑证书", 650, 320, "../Prove/ProveEdit", "#editProve-buttons");
    }
}

//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "证书")) {
            ProveID = rows[0].ProveID;
            Easy.OpenDialogEvent("#editProve", "编辑证书", 600, 380, "../Prove/ProveEdit", "#editProve-buttons");
        }
    }
}

//修改证书状态
editStatus = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "editStatus")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "证书")) {
            ProveID = rows[0].ProveID;
            Easy.OpenDialogEvent("#editProveStatus", "编辑证书状态", 400, 180, "../Prove/ProveStatusEdit", "#editProveStatus-buttons");
        }
    }
}

//查看
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "证书")) {
            ProveID = rows[0].ProveID;
            Easy.OpenDialogEvent("#viewProve", "查看证书", 800, 600, "../Prove/ProveView", "#viewProve-buttons");
        }
    }
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../Prove/DownloadProve",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}

//导出模板
deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/证书状态变更模板.xls";
    }
}

//绑定更新状态方法
bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "证书")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Prove/GetUpdateStatus", rows[0].ProveID, "1", "#grid");
    }
}

upProveNum = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upProveNum")) {
        ProveID = "0";
        Easy.OpenDialogEvent("#upProveNum", "导入证书编号", 800, 600, "../Prove/ProveNumUpload", "#upProveNum-buttons");
    }
};

downProveNumTemplate = function () {

    if (Easy.bindPowerValidationEvent(menuId, "1", "downProveNumTemplate")) {
        location.href = "../ModelExcel/证书编号上传模板.xls";
    }
};

bindSaveUplodMethod = function () {
    $("#btnupProveNum").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有交费人信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload1", "../Prove/UploadProveNum", "#btnupProveNum", "#grid", "1", "upload", "#gridStudent", data.rows, "#stuLayout");
    });
}

editProveNum = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upProveNum")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        //验证是否选中行
        if (Easy.checkRow(rows, "证书")) {
            selectRow = rows[0];
            Easy.OpenDialogEvent("#editProveNum", "编辑证书编号", 320, 200, "../Prove/ProveNumEdit", "#editProveNum-buttons");
        }
    }
}