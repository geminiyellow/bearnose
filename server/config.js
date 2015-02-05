module.exports = {
  database: {
    MONGODB: process.env.MONGODB || 'localhost',
  },
  api: {
    oschina: {
      CLIENT_ID : process.env.CLIENT_ID_OSCHINA || 'g8SJmBsS3zkhDrVhfptp', //  TODO: for debug.
      CLIENT_SECRET: process.env.CLIENT_SECRET_OSCHINA || 'e1ZzgXW7XEdwqvPlnfkwRmrpNsXBlZod',
      REDIRECT_URI : process.env.REDIRECT_URI || 'https://microsb-geminiyellow-6.c9.io/auth/oschina/callback'
    }
  },
  PORT: process.env.PORT || 3000,
  TOKEN_SECRET: process.env.TOKEN_SECRET || 'pick a hard to guess string'
};