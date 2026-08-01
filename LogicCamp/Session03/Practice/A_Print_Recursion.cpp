#include <bits/stdc++.h>
using namespace std;

void print(int n){
    if(n == 0) return;
    print(n - 1);
    cout << "I love Recursion" << "\n";
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int n; cin >> n;
    print(n);

    return 0;
}