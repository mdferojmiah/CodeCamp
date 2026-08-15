#include <bits/stdc++.h>
using namespace std;

struct Node{
    int value;
    Node* next;
    Node* prev;

    Node(int value){
        this->value = value;
        this->next =  nullptr;
        this->prev = nullptr;
    }
};

class LinkedList{
public:
    Node *head =  nullptr;
    Node *tail = nullptr;

    void insertAtTail(int value){
        Node *newNode = new Node(value);

        if(head == nullptr){
            head = newNode;
            tail = newNode;
            return;
        }
        tail->next = newNode;
        newNode->prev = tail;
        tail = newNode;
    }

    void deleteFromTail(){
        if(head == nullptr){
            cout << "Nothing to delete\n";
            return;
        }
        if(head == tail){
            Node *targetNode = head;
            head = nullptr;
            tail = nullptr;
            delete targetNode;
            return;
        }

        Node *targetNode = tail;
        tail = tail->prev;
        tail->prev->next = nullptr;
        delete targetNode;
    }

    void deleteFromHead(){
        if(head == nullptr){
            cout << "Nothing to delete\n";
            return;
        }
        if(head == tail){
            Node *targetNode = head;
            head = nullptr;
            tail = nullptr;
            delete targetNode;
            return;
        }

        Node *targetNode = head;
        head = head->next;
        head->prev = nullptr;
        delete targetNode;
    }
};

class Stack{
public:
    LinkedList linkedList;
    
    int sz = 0;

    int size(){
        return sz;
    }

    bool isEmpty(){
        return sz == 0;
    }

    void push(int value){
        linkedList.insertAtTail(value);
        sz++;
    }

    int top(){
        return linkedList.tail->value;
    }

    int pop(){
        int tailValue = linkedList.tail->value;
        linkedList.deleteFromTail();
        sz--;
        return tailValue;
    }
};

class Queue{
public:
    LinkedList linkedList;
    int sz =  0;

    int size(){
        return sz;
    }

    bool isEmpty(){
        return sz == 0;
    }

    void push(int value){
        linkedList.insertAtTail(value);
        sz++;
    }

    int front(){
        return linkedList.head->value;
    }

    int pop(){
        int returnValue = linkedList.head->value;
        linkedList.deleteFromHead();
        sz--;
        return returnValue;
    }
};

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    #pragma region vector implementaion of stack & queue
    // stack using vector
    // push() -> vector.push_back();
    // top() -> vector[vector.size() - 1];
    // pop() -> vector.pop_back();

    // queue using vector
    // push() -> vector.push_back();
    // front() -> vector[0];
    // pop() -> vector.erase(vector.begin(), 0);
    #pragma endregion

    #pragma region stack
    // Stack st;
    // st.push(10);
    // st.push(20);
    // st.push(30);

    // cout << "size: " << st.size() << "\n";
    // cout << "top: " << st.top() << '\n';

    // cout << "poped: " << st.pop() << '\n';

    // cout << "size: " << st.size() << "\n";
    // cout << "top: " << st.top() << '\n';
    #pragma endregion

    #pragma region queue
    Queue q;
    q.push(100);
    q.push(200);
    q.push(300);
    
    cout << "Size: " << q.size() << endl;
    cout << "Front: " << q.front() << endl;

    cout << "Popped: " << q.pop() << endl;
    cout << "Front: " << q.front() << endl;
    cout << "Size: " << q.size() << endl;
    #pragma endregion

    return 0;
}