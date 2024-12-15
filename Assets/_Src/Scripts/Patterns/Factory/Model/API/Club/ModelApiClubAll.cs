using System;
using System.Collections.Generic;

[Serializable]
public class ModelApiClubAll
{
    public int total;
    public int offset;
    public int limit;
    public List<ModelApiClubData> data;

    // public void MockUp()
    // {
    //     total = 10;
    //     offset = 0;
    //     limit = 20;
    //     data = new List<ModelApiClubData>();
    //     for (int i = 0; i < 10; i++)
    //     {
    //         data.Add(new ModelApiClubData().MockUp(i));
    //     }
    // }
}