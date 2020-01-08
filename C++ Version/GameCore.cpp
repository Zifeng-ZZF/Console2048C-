#include "GameCore.h"
#include <algorithm>
#include <ctime>

Position::Position(int r, int c)
{
    rIndex = r;
    cIndex = c;
}

GameCore::GameCore()
{
    m_size = 4;
    isChanged = false;
    random = 0;

    map = new int *[m_size];
    for (int i = 0; i < m_size; ++i)
        map[i] = new int[m_size];

    preMap = new int *[m_size];
    for (int i = 0; i < m_size; ++i)
        preMap[i] = new int[m_size];

    rowColBuffer = new int[m_size];
    zeroRemoveBuffer = new int[m_size];
    emptyLocationList = new std::vector<Position>;
}

GameCore::~GameCore()
{
    for (int i = 0; i < sizeof(map) / sizeof(map[0]); i++)
    {
        delete[] map[i];
    }
    delete[] map;

    for (int i = 0; i < sizeof(preMap) / sizeof(preMap[0]); i++)
    {
        delete[] preMap[i];
    }
    delete[] preMap;

    delete[] rowColBuffer;
    delete[] zeroRemoveBuffer;
    delete emptyLocationList;
}

int **GameCore::GetMap()
{
    return map;
}

void GameCore::RemoveZeros()
{
    std::fill_n(zeroRemoveBuffer, 4, 0);
    int j = 0;
    for (int i = 0; i < m_size; i++)
    {
        if (rowColBuffer[i] != 0)
        {
            zeroRemoveBuffer[j] = rowColBuffer[i];
            j++;
        }
    }
    memcpy(rowColBuffer, zeroRemoveBuffer, sizeof(int) * 4);
}

void GameCore::Merge()
{
    RemoveZeros();

    for (int i = 0; i < m_size - 1; i++)
    {
        if (rowColBuffer[i] != 0 && rowColBuffer[i] == rowColBuffer[i + 1])
        {
            rowColBuffer[i] += rowColBuffer[i + 1];
            rowColBuffer[i + 1] = 0;
        }
    }
    RemoveZeros();
}

void GameCore::FindAllEmpty()
{
    emptyLocationList->clear();
    for (int r = 0; r < m_size; r++)
    {
        for (int c = 0; c < m_size; c++)
        {
            if (map[r][c] == 0)
            {
                Position p(r, c);
                emptyLocationList->push_back(p);
            }
        }
    }
    // std::cout << "\n" << emptyLocationList->size() << "\n";
}

void GameCore::GenerateRandom()
{
    FindAllEmpty();

    srand((int)time(0));
    int random_index = rand() % emptyLocationList->size(); // 按概率返回4、2:在0到10中拿一个数的概率是百分之十，如果是则返回4，否则2
    std::cout << "my random index: " << random_index << std::endl;
    Position pos = emptyLocationList->at(random_index);

    srand((int)time(0));
    random = (rand() % 10 == 1 ? 4 : 2);
    std::cout << "random: " << random << std::endl;

    map[pos.rIndex][pos.cIndex] = random;
}

void GameCore::MoveUp()
{
    for (int c = 0; c < m_size; c++)
    {
        //Get one columne
        for (int r = 0; r < m_size; r++)
        {
            rowColBuffer[r] = map[r][c];
        }

        Merge();

        //Update map
        for (int r = 0; r < m_size; r++)
        {
            map[r][c] = rowColBuffer[r];
        }
    }
}

void GameCore::MoveLeft()
{
    for (int r = 0; r < m_size; r++)
    {
        //Get one row
        for (int c = 0; c < m_size; c++)
        {
            rowColBuffer[c] = map[r][c];
        }

        Merge();

        //Update
        for (int c = 0; c < m_size; c++)
        {
            map[r][c] = rowColBuffer[c];
        }
    }
}

void GameCore::MoveDown()
{
    for (int c = 0; c < m_size; c++)
    {
        //Get one columne
        int n = m_size - 1;
        for (int r = 0; r < m_size; r++)
        {
            rowColBuffer[n] = map[r][c];
            n--;
        }

        Merge();

        //Update map
        n = m_size - 1;
        for (int r = 0; r < m_size; r++)
        {
            map[r][c] = rowColBuffer[n];
            n--;
        }
    }
}

void GameCore::MoveRight()
{
    for (int r = 0; r < m_size; r++)
    {
        int n = m_size - 1;
        for (int c = 0; c < m_size; c++)
        {
            rowColBuffer[n] = map[r][c];
            n--;
        }

        Merge();

        n = m_size - 1;
        for (int c = 0; c < m_size; c++)
        {
            map[r][c] = rowColBuffer[n];
            n--;
        }
    }
}

bool GameCore::CheckSameMap()
{
    for (int r = 0; r < m_size; r++)
    {
        for (int c = 0; c < m_size; c++)
        {
            if (map[r][c] != preMap[r][c])
            {
                return false;
            }
        }
    }
    return true;
}

void GameCore::NumMove(MoveDirection direction)
{
    // memcpy(preMap, map, sizeof(int) * 16);
    for (int r = 0; r < m_size; r++)
    {
        for (int c = 0; c < m_size; c++)
        {
            preMap[r][c] = map[r][c];
        }
    }

    switch (direction)
    {
    case MoveDirection::Up:
        MoveUp();
        break;
    case MoveDirection::Down:
        MoveDown();
        break;
    case MoveDirection::Left:
        MoveLeft();
        break;
    case MoveDirection::Right:
        MoveRight();
        break;
    default:
        break;
    }

    //check if map changed then generate random number
    if(!CheckSameMap())
    {
        GenerateRandom();
    }
}

void GameCore::CheckGameEnd()
{
    
}