const API_BASE = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'
const TOKEN = import.meta.env.VITE_API_TOKEN ?? 'my-demo-token' // fallback

const headers = {
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${TOKEN}`
}

export async function getAllUsers() {
  const res = await fetch(`${API_BASE}/api/v1/Users/GetAll`, { headers })
  if (!res.ok) throw new Error(`Error ${res.status}`)
  return res.json()
}

export async function getUserById(id) {
  const res = await fetch(`${API_BASE}/api/v1/Users/GetById/${id}`, { headers })
  if (!res.ok) throw new Error(`Error ${res.status}`)
  return res.json()
}

export async function createUser(payload) {
  const res = await fetch(`${API_BASE}/api/v1/Users/Create`, {
    method: 'POST',
    headers,
    body: JSON.stringify(payload)
  })
  if (!res.ok) {
    const text = await res.text().catch(()=>null)
    throw new Error(text || `Error ${res.status}`)
  }
  return res.json()
}

export async function updateUser(id, payload) {
  const res = await fetch(`${API_BASE}/api/v1/Users/Update/${id}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(payload)
  })
  if (!res.ok) {
    const text = await res.text().catch(()=>null)
    throw new Error(text || `Error ${res.status}`)
  }
  return res.ok
}

export async function deleteUser(id) {
  const res = await fetch(`${API_BASE}/api/v1/Users/Delete/${id}`, {
    method: 'DELETE',
    headers
  })
  if (!res.ok) throw new Error(`Error ${res.status}`)
  return res.ok
}
