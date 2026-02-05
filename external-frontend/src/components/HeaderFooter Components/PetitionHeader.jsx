import { Link } from 'react-router-dom';
import logo from '../../assets/court of arms.png';
import React from 'react';
export default function PetitionHeader () {
    return (
        <div >
            <nav class="navbar navbar-expand-lg navbar-dark bg-success">
        <div class="container">
        <Link to="/" className="navbar-brand">
          <img src={logo} alt="Logo" width="140" height="70" />
        </Link>
        </div>
    </nav>

            <header class="bg-light py-4">
        <div class="container">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0">
                    <li class="breadcrumb-item">
                        <Link to="/">Home</Link>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">Create Petition</li>
                </ol>
            </nav>
            <h1 class="mt-3">Create a New Petition</h1>
            <p class="lead">Fill out the form below to submit your petition to Parliament</p>
        </div>
    </header>
</div>
    )
}