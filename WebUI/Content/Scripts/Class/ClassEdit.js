bindSelectInfo = function () {
    if (ClassID != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Class/SelectClass", ClassID);
            var c = JSON.parse(result.Message)[0];
            $("#ClassID").textbox("setValue", c.ClassID);
            $("#cProfessionID").textbox("setValue", c.ProfessionID);
            $("#ClassName").textbox("setValue", c.Name);
        }, 1);
    }
    else {
        $("#cProfessionID").val(ProfessionID);
    }
}
bindSelectInfo();