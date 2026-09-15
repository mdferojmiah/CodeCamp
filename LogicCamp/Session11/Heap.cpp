#include <bits/stdc++.h>
using namespace std;

vector<int> vv;

int get_parent_index(int index){
    return (index - 1) / 2;
}

void upheapify(int last_index){
    if(last_index <= 0) return;
    int parent_index = get_parent_index(last_index);
    if(vv[parent_index] < vv[last_index]){
        swap(vv[parent_index], vv[last_index]);
        upheapify(parent_index);
    }
}

void push(int number){
    vv.push_back(number);
    upheapify(vv.size() - 1);
}

int get_left_index(int index){
    return 2 * index + 1;
}

int get_right_index(int index){
    return 2 * index + 2;
}

void downheapify(int index){
    int left_index = get_left_index(index);
    int right_index =  get_right_index(index);

    if(left_index < vv.size() && right_index < vv.size()){
        int largest_child = max(vv[left_index], vv[right_index]);
        if(vv[index] < largest_child){
            if(vv[left_index] == largest_child){
                swap(vv[index], vv[left_index]);
                downheapify(left_index);
            }else{
                swap(vv[index], vv[right_index]);
                downheapify(right_index);
            }
        }
    }else if(left_index < vv.size()){
        if(vv[index] < vv[left_index]){
            swap(vv[index], vv[left_index]);
            downheapify(left_index);
        }
    }
}

void pop(){
    int last_index = vv.size() - 1;
    swap(vv[0], vv[last_index]);
    vv.pop_back();
    downheapify(0);
}

int top(){
    return vv[0];
}

void print(){
    for(int i = 0; i < vv.size(); i++){
        cout << vv[i] << " ";
    }
    cout << "\n";
}

int main()
{
    push(2);
    push(13);
    push(14);
    push(15);
    push(18);
    push(19);
    push(20);
    push(25);

    print();
    // push(26);
    // print();
    pop();
    print();
    
    cout << "\n" << top() << "\n";
    return 0;
}