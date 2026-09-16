var numbersTest = new List<int> { 1, 3, 5, 7, 9, 11, 13, 15 };
Console.WriteLine(BinarySearchWithSteps(numbersTest, 9));
return;

int BinarySearchWithSteps(List<int> numbers, int target)
{
    if(numbers.Count == 0)
    {
        return -1;
    }

    var stepsCount = 0; 
    var left = 0;
    var right = numbers.Count - 1;
    while (left <= right)
    {
        stepsCount++;
        
        var mid = (left + right) / 2;
        if (numbers[mid] == target)
            return stepsCount;
        if (numbers[mid] < target)
            left =  mid + 1;
        else
            right = mid - 1;
    }
    
    return -1;
}