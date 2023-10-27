local Sys1 ={}
function Sys1.Fun1()
    local obj = GameObject('test')
    local a = 100
    a = a + 1
    -- body
    print("Sys1.Fun1()" .. a)
end
return Sys1