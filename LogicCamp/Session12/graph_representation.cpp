#include <bits/stdc++.h>
using namespace std;

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);


    #pragma region Adjacent_list

    int node, edge; cin >> node >> edge;
    vector<pair<int, int>> adjacentList[node+1];

    for(int i = 1; i <= node; i++){
        int currentNode, noOfEdges; cin >> currentNode >> noOfEdges;
        for(int j = 0; j < noOfEdges; j++){
            int adjacentElement, weight; cin >> adjacentElement >> weight;
            adjacentList[currentNode].push_back({adjacentElement, weight});
        }
    }

    for(int i = 1; i <= node; i++){
        cout << i << ": ";
        for(int j = 0; j < adjacentList[i].size(); j++){
            cout << "(" << adjacentList[i][j].first << ", " << adjacentList[i][j].second << "), ";
        }
        cout << '\n';
    }

    #pragma endregion

    #pragma region twoD_Matrix
    // int node, edge;
    // cin >> node >> edge;
    // int grid[node+1][node+1];

    // for(int i = 1; i <= node; i++){
    //     for(int j = 1; j <= node; j++){
    //         grid[i][j] = 0;
    //     }
    // }

    // for(int i = 1; i <= node; i++){
    //     int currNode, noOfEdges;
    //     cin >> currNode >> noOfEdges;
    //     for(int j = 0; j < noOfEdges; j++){
    //         int adjacentNode, weight; cin >> adjacentNode >> weight;
    //         grid[currNode][adjacentNode] = weight;
    //         //for unidirected graph
    //         //grid[adjacentNode][currNode] = weight;
    //     }
    // }

    // cout << "   ";
    // for(int i = 1; i <= node; i++){
    //     cout << i << " ";
    // }
    // cout << '\n';

    // for(int i = 1; i <= node; i++){
    //     cout << i << ": ";
    //     for(int j = 1; j <= node; j++){
    //         cout << grid[i][j] << " ";
    //     }
    //     cout << '\n';
    // }
    #pragma endregion


    return 0;
}