type SampleType is record
  var field1 is 10;
  var field2 is 10.0;
end

type FirstRoutine is record
  var field3 is 10;
end

routine FirstRoutine() : integer is
  var st : SampleType

  var a is 1.0
  var b is st.field2

  var c is 0.0

//  for i in 1..10 loop
//    c := c + a + b
//  end

  return c
end
