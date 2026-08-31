#include <bits/stdc++.h>
using namespace std;

class Node
{
    public:
        int val;
        Node* left;
        Node* right;

    Node(int val){
        this->val = val;
        this->left = nullptr;
        this->right = nullptr;
    }
    
};

#pragma region DFS
void pre_order(Node* node){
    if(!node) return;
    cout << node->val << " ";
    pre_order(node->left);
    pre_order(node->right);
}

void in_order(Node* node){
    if(node->left != nullptr) in_order(node->left);
    cout << node->val << " ";
    if(node->right != nullptr) in_order(node->right);
}

void post_order(Node* node){
    if(node->left != nullptr) post_order(node->left);
    if(node->right != nullptr) post_order(node->right);
    cout << node->val << " ";
}
#pragma endregion


#pragma region BFS
void BFS(Node* root){
    queue<Node*> q;
    q.push(root);

    while(!q.empty()){
        Node* curr = q.front();
        cout << curr->val << " ";
        q.pop();
        if(curr->left != nullptr) q.push(curr->left);
        if(curr->right != nullptr) q.push(curr->right);
    }
}

void BFSWithLevel(Node* root){
    queue<pair<Node*, int>> q;
    q.push({root, 0});

    while(!q.empty()){
        Node* curr = q.front().first;
        int level = q.front().second;
        q.pop();

        cout << curr->val << "-> " << level << '\n';
        
        if(curr->left != nullptr) q.push({curr->left, level + 1});
        if(curr->right != nullptr) q.push({curr->right, level + 1});
    }
}
#pragma endregion

Node* buildTree(vector<string> &v){
    if(v.empty() || v[0] == "null" || v[0] == "") return nullptr;

    Node* root = new Node(stoi(v[0]));
    queue<Node*> q;
    q.push(root);

    int i = 1;
    while(!q.empty() && i < v.size()){
        Node* curr = q.front();
        q.pop();

        if(i < v.size()){
            if(v[i] != "null"){
                curr->left = new Node(stoi(v[i]));
                q.push(curr->left);
            }
            i++;
        }

        if(i < v.size()){
            if(v[i] != "null"){
                curr->right = new Node(stoi(v[i]));
                q.push(curr->right);
            }
            i++;
        }
    }
    return root;
}

int main()
{
    ios::sync_with_stdio(0);
    cin.tie(0);

    #pragma region insert a tree
    //reading whole input line as string
    string line;
    getline(cin, line);
    //converting the line into string stream
    stringstream ss(line);
    //reading the string stream and inserting the value into a vector
    vector<string> v;
    while(getline(ss, line, ',')){
        v.push_back(line);
    }

    //building tree
    Node* root = buildTree(v);

    //travesing the tree
    pre_order(root);
    cout << "\n";
    BFS(root);
    #pragma endregion

    #pragma region manual tree forming
    // Node* one = new Node(1);
    // Node* two = new Node(2);
    // Node* three = new Node(3);
    // Node* four = new Node(4);
    // Node* five = new Node(5);
    // Node* six = new Node(6);

    // one->left = two;
    // one->right = three;

    // two->left = four;
    // two->right = five;

    // three->left = six;
    #pragma endregion

    #pragma region DFS-visualization
    // cout << "pre-order: "; pre_order(one); cout << "\n";
    // cout << "in-order: "; in_order(one); cout << "\n";
    // cout << "pre-order: "; post_order(one); cout << "\n";
    #pragma endregion

    #pragma region BFS-visualization
    // cout << "BFS: ";
    // BFS(one);
    // cout << '\n';

    // BFSWithLevel(one);
    #pragma endregion

    return 0;
}