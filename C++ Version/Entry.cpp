#include <iostream>
#include "GameCore.h"

static GameCore core;
static bool isWin = false;
static bool isFail = false;

void DrawMap()
{
    int** myMap = core.GetMap();
    int size = 4;
    for (int r = 0; r < size; r++)
    {
        for (int c = 0; c < size; c++)
        {
            if(myMap[r][c] == 2048)
            {
                isWin = true;
            }
            std::cout << myMap[r][c] << " ";
        }
        std::cout << "\n";
    }
}

void myMove()
{
    char ch;
    std::cin >> ch;
    switch (ch)
    {
    case 'w':
        core.NumMove(MoveDirection::Up);
        break;
    case 'a':
        core.NumMove(MoveDirection::Left);
        break;
    case 's':
        core.NumMove(MoveDirection::Down);
        break;
    case 'd':
        core.NumMove(MoveDirection::Right);
        break;
    default:
        break;
    }
}

int main()
{
    core.GenerateRandom();
    core.GenerateRandom();

    DrawMap();

    while(true)
    {
        myMove();
        DrawMap();

        if(isWin)
        {
            std::cout << "You won! \n";
            break;
        }

        if(core.emptyLocationList->size() == 0 || isFail)
        {
            std::cout << "You failed! \n";
            break;
        }
    }

    return 0;
}