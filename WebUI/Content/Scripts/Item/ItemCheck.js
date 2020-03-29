bindData = function () {
    $('#itemgrid').datagrid({
        data: JSON.parse(itemData),
        singleSelect: true,
        columns: [[
            { field: 'DeptName', title: '主体', width: 200 },
            { field: 'ParentName', title: '父级', width: 200 },
            { field: 'Name', title: '字段', width: 200 },
            {
                field: 'Type', title: '类型', width: 80
            }
        ]], toolbar: [{
            iconCls: 'icon-save',
            text: '导出',
            handler: function () {
                Easy.DeriveFile("#itemgrid", "问题收费项目.xls");
            }
        }]

    });
    $('#detailgrid').datagrid({
        data: JSON.parse(detailData),
        singleSelect: true,
        columns: [[
            { field: 'DeptName', title: '主体', width: 200 },
            { field: 'ParentName', title: '收费项', width: 200 },
            { field: 'Name', title: '字段', width: 200 },
            {
                field: 'Type', title: '类型', width: 80
            }
        ]], toolbar: [{
            iconCls: 'icon-save',
            text: '导出',
            handler: function () {
                Easy.DeriveFile("#detailgrid", "问题收费项目明细.xls");
            }
        }]
    });

}
bindData();