#include <bits/stdc++.h>
using namespace std;

void rev(int arr[], int n){
    if(n == 1) return;
    rev(arr, n - 1);
    for(int i = n - 1; i > 0; i--){
        swap(arr[i], arr[i - 1]);
    }
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int n; cin >> n;
    int arr[n];
    for(int i = 0; i < n; i++) cin >> arr[i];

    rev(arr, n);

    for(int i = 0; i < n; i++) cout << arr[i] << " ";

    return 0;
}