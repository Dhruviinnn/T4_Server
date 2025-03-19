export const userFetcher = (user, setUser) => {
    if (!user.userId) {
        console.log('Fetching needed');
        fetch('http://localhost:3000/api/user/get',
            {
                method: 'GET',
                credentials: "include",
            }
        )
            .then(res => res.json())
            .then(({ user }) => { console.log(user); setUser(user) })
    }
    console.log(user);
}