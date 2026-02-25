using System;
using System.Security.Cryptography.X509Certificates;

namespace MatchmakingSystem;

public class Individual
{
    private int _id;
    private string _gender;
    private int _age;
    private string _intro;
    private List<string> _habits;
    private int _x;
    private int _y;
    

    public Individual(int id, string gender, int age, string intro, List<string> habits, int x, int y)
    {
        _id = id;
        _gender = gender;
        _age = age;
        _intro = intro;
        _habits = habits;
        _x = x;
        _y = y;
    }
}
