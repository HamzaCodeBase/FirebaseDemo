using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace FirebaseDemo;

public class FirebaseService
{
    private readonly FirestoreDb _firestoreDb;

    public FirebaseService(IConfiguration confif)
    {
        var projectId = confif["Firebase:ProjectId"];
        var filePath = confif["Firebase:FilePath"];

        var credentials = GoogleCredential.FromFile(filePath);
        var client = new FirestoreClientBuilder
        {
            Credential = credentials
        }.Build();
        _firestoreDb = FirestoreDb.Create(projectId, client);
    }

    public async Task AddUserAsync(string name, int age)
    {
        DocumentReference docRef = _firestoreDb.Collection("users").Document();
        Dictionary<string, object> user = new()
    {
        { "name", name },
        { "age", age }
    };
        await docRef.SetAsync(user);
    }

    public async Task<Dictionary<string, object>> GetUserAsync(string userId)
    {
        DocumentReference docRef = _firestoreDb.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        return snapshot.Exists ? snapshot.ToDictionary() : null;
    }

    public async Task<List<Dictionary<string, object>>> GetAllUsersAsync()
    {
        QuerySnapshot snapshot = await _firestoreDb.Collection("users").GetSnapshotAsync();
        return snapshot.Documents.Select(doc =>
        {
            var data = doc.ToDictionary();
            data["id"] = doc.Id; // Include document ID in the result
            return data;
        }).ToList();
    }

    public async Task UpdateUserAsync(string userId, string name, int age)
    {
        DocumentReference docRef = _firestoreDb.Collection("users").Document(userId);
        Dictionary<string, object> updates = new()
        {
            { "name", name },
            { "age", age }
        };
        await docRef.UpdateAsync(updates);
    }

    public async Task DeleteUserAsync(string userId)
    {
        DocumentReference docRef = _firestoreDb.Collection("users").Document(userId);
        await docRef.DeleteAsync();
    }
}
