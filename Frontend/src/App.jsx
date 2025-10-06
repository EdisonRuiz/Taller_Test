import React from 'react'
import UsersList from './components/UsersList'
import UserForm from './components/UserForm'

export default function App() {
  return (
    <div className="container">
      <h1>User Management (Demo)</h1>
      <div className="panel">
        <UserForm />
        <UsersList />
      </div>
    </div>
  )
}
