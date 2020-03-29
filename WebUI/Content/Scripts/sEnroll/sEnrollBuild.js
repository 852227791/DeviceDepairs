//绑定表单数据
initsOrderGrid = function () {
    $("#sOrderGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        columns: [[
            { field: "Result", title: "生成结果", width: 100, sortable: true },
            { field: "Remark", title: "备注", width: 120, sortable: true },
            { field: "StuName", title: "学生姓名", width: 60, sortable: true },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "Level", title: "学习层次", width: 60, sortable: true },
            { field: "Major", title: "专业", width: 100, sortable: true },
            { field: "Scheme", title: "缴费方案", width: 150, sortable: true },
            { field: "IDCard", title: "身份证号", width: 150, sortable: true }
        ]],
        toolbar: [{
            iconCls: 'icon-down',
            text: '导出',
            handler: GoOutResultExcel
        }],
        rowStyler: function (index, row) {
            if (row.Result == "缴费单生成失败" || row.Result == "配品生成失败") {
                return 'color:red;';//rowStyle是一个已经定义了的ClassName(类名)
            }
        }
    });
}

initFunction = function () {
    $("#titleTips").html("正在生成缴费单...，请勿关闭浏览器！");
    $('#btnsEnrollBuild').linkbutton('disable');
    var array = sEnrollsProfessionIDs.split(',');
    for (var i = 0; i < array.length; i++) {
        $.ajax({
            type: "post",
            url: "../sEnroll/BuildsFee",
            async: false,
            data: { ID: array[i] },
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var array = JSON.parse(data.Message);
                    for (var i = 0; i < array.length; i++) {
                        $('#sOrderGrid').datagrid('appendRow', {
                            Result: array[i].Result,
                            Remark: array[i].Remark,
                            StuName: array[i].StuName,
                            Year: array[i].Year,
                            Month: array[i].Month,
                            Level: array[i].Level,
                            Major: array[i].Major,
                            Scheme: array[i].Scheme,
                            IDCard: array[i].IDCard
                        });
                    }
                }
            },
            error: function () {
                Easy.centerShow("系统消息", "出现未知错误，请联系管理员", 3000);
            }
        });
        if (i == array.length - 1) {
            //执行完毕后改变提示信息，恢复关闭按钮
            $("#titleTips").html("");
            $("#btnsEnrollBuild").linkbutton('enable');
        }
    }
    $("#grid").datagrid("load");//刷新表格
}

GoOutResultExcel = function () {
    Easy.DeriveFile("#sOrderGrid", "生成结果.xls");
}

initsOrderGrid();

initFunction();