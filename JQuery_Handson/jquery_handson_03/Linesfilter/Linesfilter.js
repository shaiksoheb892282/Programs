$(document).ready(function(){
    $("#search").click(function(){
    
        var ser= $("#text1").val();
        var ar= ser.split(",");
        var ans=""
        var main= $("#text2").val();
        var text2= main.split(/\n/);
       
       
        for(var i=0;i<text2.length;i++){
            for(var j=0;j<ar.length;j++){
                if(text2[i].includes(ar[j])){
                    var first=""
                    var second=""
                    var sp=text2[i].split(ar[j])
                    first=sp[0]
                    second=sp[1]
                    ans+=first+"<b>"+ar[j]+"</b>"+second+", "

                }

            }
        }
        $("#text3").val(ans)
    })
    
});