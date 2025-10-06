import React, { useEffect, useState } from 'react'
import { getAllUsers, deleteUser } from '../api'
import UserForm from './UserForm'

export default function UsersList() {
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)
  const [editingUserId, setEditingUserId] = useState(null)

  const load = async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await getAllUsers()
      setUsers(data)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar usuario?')) return
    try {
      await deleteUser(id)
      await load()
    } catch (err) {
      alert('Error al eliminar: ' + err.message)
    }
  }

  return (
    <div className="users-list">
      <h2>Usuarios</h2>
      <button onClick={load}>Refrescar</button>
      {loading && <p>Cargando...</p>}
      {error && <p className="error">{error}</p>}
      <table>
        <thead>
          <tr>
            <th>Id</th><th>First</th><th>Last</th><th>Email</th><th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {users?.length === 0 && <tr><td colSpan="5">No hay usuarios</td></tr>}
          {users?.map(u => (
            <tr key={u.id}>
              <td>{u.id}</td>
              <td>{u.firstName}</td>
              <td>{u.lastName}</td>
              <td>{u.email}</td>
              <td>
                <button onClick={() => setEditingUserId(u.id)}>Editar</button>
                <button onClick={() => handleDelete(u.id)}>Eliminar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {editingUserId && (
        <div className="modal">
          <div className="modal-content">
            <button className="close" onClick={() => setEditingUserId(null)}>×</button>
            <UserForm userId={editingUserId} onSaved={() => { setEditingUserId(null); load() }} />
          </div>
        </div>
      )}
    </div>
  )
}
