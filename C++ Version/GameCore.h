#ifndef GAME_CORE
#define GAME_CORE

#include <iostream>
#include <vector>
#include "MoveDirection.h"

struct Position
{
public:
    int rIndex;
    int cIndex;
    Position(int rIndex, int cIndex);
};

class GameCore
{
private:
    int **map;
    int *rowColBuffer;
    int *zeroRemoveBuffer;
    int **preMap;
    int m_size;
    int random;
public:
    std::vector<Position> *emptyLocationList;
    GameCore();
    ~GameCore();
    bool isChanged;
    void RemoveZeros();
    void Merge();
    int** GetMap();
    void MoveUp();
    void MoveDown();
    void MoveLeft();
    void MoveRight();
    bool CheckSameMap();
    void NumMove(MoveDirection);
    void FindAllEmpty();
    void GenerateRandom();
    void CheckGameEnd();
};

#endif